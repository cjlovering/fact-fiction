import re
import sys
from sklearn.base import BaseEstimator, TransformerMixin
from nltk.tokenize import word_tokenize

class SentenceNormalizer(BaseEstimator, TransformerMixin):

    def __init__(self, dictionary=None, discarded_tokens=set()):
        self.patterns = [
            (r'^([£€])?[-+]?\d+(,\d+)*(\.\d+)?(bn|m)?$', r'<NUM>'),                           # Turn numbers to <NUM>
            (r'^([£€])?[-+]?\d+(,\d+)*(\.\d+)?\-(?P<rest>[a-zA-Z\-]+)$', r'<NUM>-\g<rest>'),  # Turn numbers-something to <NUM>-something
            (r'^([0-9]|0[0-9]|1[0-9]|2[0-3]):[0-5][0-9]$', r'<TIME>'),                        # Turn time (HH:MM) to <TIME>
            (r'^(\/\/)?(\w+\.)?[\w-]+\.(com|edu|org|net)\/?(\/[\w\/]+)?$', '<URL>'),          # Turn simple url to <URL>
            (r"^''(.+)$", r'\1'),                                                             # Remove '' before a word
            (r"^'([\w-][\w-][\w-]+)$", r'\1')                                                 # Remove ' before a word longer than 3
        ]
        self.dictionary = set(dictionary) if dictionary is not None else dictionary
        self.discarded_tokens = set(discarded_tokens)

    def fit_transform(self, sentences):
        new_sentences = [''] * len(sentences)
        for i, s in enumerate(sentences):
            # Hacky replacement to fix text returned from MTusk
            s = s.replace('^', ',')
            tokens = word_tokenize(s)
            # Filter discarded tokens
            tokens = [t for t in tokens if t not in self.discarded_tokens]
            # Replace tokens with pre-defined rules
            tokens = [self.__match_and_replace(t) for t in tokens]
            new_sent = ' '.join(tokens)
            new_sentences[i] = new_sent
        return new_sentences

    def __match_and_replace(self, token):
        # Scan regex rules
        for p, t in self.patterns:
            token, change = re.subn(p, t, token)
            if change > 0:
                return token
        # If a dictionary is provided, replace unknown tokens with <UNK>
        if not self.__is_in_dictionary(token):
            return '<UNK>'
        return token

    def __is_in_dictionary(self, word):
        if self.dictionary is None:
            return True
        return word         in self.dictionary\
            or word.title() in self.dictionary\
            or word.lower() in self.dictionary\
            or word.upper() in self.dictionary

    