import unittest
from ..normalize_sentences import SentenceNormalizer

class SentenceNormalizerTest(unittest.TestCase):
    
    def setUp(self):
        DICTIONARY = set([
            'You', 'I', 'there', 'are', 'am', 'not', 'a', 'butterfly', ',' ,'.'
        ])
        self.normalizer = SentenceNormalizer()
        self.normalizer_dict = SentenceNormalizer(dictionary=DICTIONARY)

    def test_sentence_tokenizer(self):
        sentences = ["he can't do it", "This book, written by J.K. Rowling, is expensive!"]
        normalized = self.normalizer.fit_transform(sentences)
        expected = ['he ca n\'t do it',
                    'This book , written by J.K. Rowling , is expensive !']
        self.assertEqual(normalized, expected)

    def test_number_replacement(self):
        sentences = ['10% 10bn 1,424,124 124,214m?', 
                     "This book, written by J.K. Rowling, costs $12.5?"]
        normalized = self.normalizer.fit_transform(sentences)
        expected = ['<NUM> % <NUM> <NUM> <NUM> ?',
                    'This book , written by J.K. Rowling , costs $ <NUM> ?']
        self.assertEqual(normalized, expected)

    def test_mtusk_comma_replacement(self):
        sentences = ['This book^ written by J.K. Rowling^ costs $12.5?']
        normalized = self.normalizer.fit_transform(sentences)
        expected = ['This book , written by J.K. Rowling , costs $ <NUM> ?']
        self.assertEqual(normalized, expected)
        
    def test_normalize_with_dictionary(self):
        sentences = ['I am not you, there is a butterfly outside.']
        normalized = self.normalizer_dict.fit_transform(sentences)
        expected = ['I am not you , there <UNK> a butterfly <UNK> .']
        self.assertEqual(normalized, expected)
