import re
import sys
from sklearn.base import BaseEstimator, TransformerMixin
from nltk.tokenize import word_tokenize

class SentenceNormalizer(BaseEstimator, TransformerMixin):

    def __init__(self, dictionary=None, discarded_tokens=set()):
        PATTERNS = [
            ('^[-+]?\d+(,\d+)*(\.\d+)?(bn|m)?$', '<NUM>')
        ]
        self.patterns = [(re.compile(p), t) for p, t in PATTERNS]
        self.dictionary = dictionary
        self.discarded_tokens = discarded_tokens

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
            if p.match(token) is not None:
                return t
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

    