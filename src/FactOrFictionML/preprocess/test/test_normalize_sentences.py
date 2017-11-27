import unittest
from ..normalize_sentences import SentenceNormalizer

class SentenceNormalizerTest(unittest.TestCase):
    
    def setUp(self):
        DICTIONARY = set([
            'You', 'I', 'there', 'are', 'am', 'not', 'a', 'butterfly', ',' ,'.'
        ])
        DISCARDED = set([
            'Bad', 'words', 'are', 'bad'
        ])
        self.normalizer = SentenceNormalizer()
        self.normalizer_dict = SentenceNormalizer(dictionary=DICTIONARY)
        self.normalizer_discard = SentenceNormalizer(discarded_tokens=DISCARDED)

    def test_sentence_tokenizer(self):
        sentences = ["he can't do it", "This book, written by J.K. Rowling, is expensive!"]
        normalized = self.normalizer.fit_transform(sentences)
        expected = ['he ca n\'t do it',
                    'This book written by J.K. Rowling is expensive']
        self.assertEqual(normalized, expected)

    def test_number_replacement(self):
        sentences = ['10% 10bn 1,424,124 124,214m?', 
                     "This book, written by J.K. Rowling, costs $12.5?",
                     "This is a 10-year project"]
        normalized = self.normalizer.fit_transform(sentences)
        expected = ['<NUM> <NUM> <NUM> <NUM>',
                    'This book written by J.K. Rowling costs <NUM>',
                    'This is a <NUM>-year project']
        self.assertEqual(normalized, expected)

    def test_time_replacement(self):
        sentences = ["It is 9:00, we're going at 10:30"]
        normalized = self.normalizer.fit_transform(sentences)
        expected = ["It is <TIME> we 're going at <TIME>"]
        self.assertEqual(normalized, expected)

    def test_url_replacement(self):
        sentences = ["Google search's url is www.google.com or google.com or www.google.com/home"]
        normalized = self.normalizer.fit_transform(sentences)
        expected = ["Google search 's url is <URL> or <URL> or <URL>"]
        self.assertEqual(normalized, expected)

    def test_mtusk_comma_replacement(self):
        sentences = ['This book^ written by J.K. Rowling^ costs $12.5?']
        normalized = self.normalizer.fit_transform(sentences)
        expected = ['This book written by J.K. Rowling costs <NUM>']
        self.assertEqual(normalized, expected)
        
    def test_normalize_with_dictionary(self):
        sentences = ['I am not you, there is a butterfly outside.']
        normalized = self.normalizer_dict.fit_transform(sentences)
        expected = ['I am not you there <UNK> a butterfly <UNK>']
        self.assertEqual(normalized, expected)

    def test_normalize_with_discarded_token(self):
        sentences = ['There are many bad person in this Bad world']
        normalized = self.normalizer_discard.fit_transform(sentences)
        expected = ['There many person in this world']
        self.assertEqual(normalized, expected)

    def test_noise_replacement(self):
        sentences = ["''This is a sentence", "'This 'is 'another one", '€1,300', '£7.5m']
        normalized = self.normalizer.fit_transform(sentences)
        expected = ['This is a sentence', "This 'is another one", '<NUM>', '<NUM>']
        self.assertEqual(normalized, expected)
