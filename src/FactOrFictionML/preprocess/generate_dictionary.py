from __future__ import print_function

import argparse
import itertools
import pandas as pd
from normalize_sentences import SentenceNormalizer

if __name__=='__main__':
    parser = argparse.ArgumentParser(description='Generate dictionary from a csv file containing sentences.')
    parser.add_argument('input_path',
                        help='path to the csv file.')
    parser.add_argument('sent_column',
                        help='name of the column containing the sentences.')
    parser.add_argument('--output', default='dictionary.txt',
                        help='path to the output file.')

    args = parser.parse_args()

    # Read sentences 
    df = pd.read_csv(args.input_path)
    sentences = df[args.sent_column]
    print('Found', sentences.size, 'sentences.')

    # Normalize them
    print('Normalizing the sentences...')
    sent_normalizer = SentenceNormalizer()
    normalized = sent_normalizer.fit_transform(sentences)
    print('Done')

    # Split sentences to tokens then merge all of them
    print('Tokenizing the sentences...')
    tokenized = [sent.split(' ') for sent in normalized]
    all_tokens = set(itertools.chain.from_iterable(tokenized))
    print('Done')

    # Write all to file
    print('Writing tokens to file...')
    with open(args.output, 'w', encoding='utf-8') as f:
        for token in sorted(all_tokens):
            f.write("%s\n" % (token))
    print('Done')
