import sys
import argparse
import re
import os
import numpy as np
from gensim.models import KeyedVectors
from common import to_word_vector

def convert(w2v_path, label_mapping, input, output, annotated):
    # create in memory dictionaries
    label_mapping = { line.rstrip('\r\n').strip():index for index, line in enumerate(label_mapping) }
    print('Loading %s...' % w2v_path)
    w2v_model = KeyedVectors.load_word2vec_format(w2v_path, binary=True)
    print('Done loading')

    # convert input
    sequenceId = 0
    for index, line in enumerate(input):
        # skip the header
        if index == 0:
            continue
        line = line.rstrip('\r\n')
        columns = line.split("\t")
        if len(columns) != 2:
            raise Exception("Need 2 columns")
        _convertSequence(w2v_model, label_mapping, columns, sequenceId, output, annotated)
        sequenceId += 1

def _convertSequence(w2v_model, label_mapping, streams, sequenceId, output, annotated):
    tokensPerStream = [[t for t in s.strip(' ').split(' ') if t != ""] for s in streams]
    maxLen = max(len(tokens) for tokens in tokensPerStream)

    # writing to the output file
    for sampleIndex in range(maxLen):
        output.write(str(sequenceId))
        for streamIndex in range(len(tokensPerStream)):
            if len(tokensPerStream[streamIndex]) <= sampleIndex:
                output.write("\t")
                continue
            
            token = tokensPerStream[streamIndex][sampleIndex]
            
            if streamIndex == 1: # S1
                value = label_mapping[token]
                onehot = ['0'] * len(label_mapping)
                onehot[value] = '1'
                output.write("\t|S" + str(streamIndex) + " " + ' '.join(onehot))
            else:
                word_vec = to_word_vector(w2v_model, token)
                output.write("\t|S" + str(streamIndex) + " ")
                output.write(' '.join([str(i) for i in word_vec]))
                
            if annotated:
                output.write(" |# " + re.sub(r'(\|(?!#))|(\|$)', r'|#', token))
        output.write("\n")

if __name__ == "__main__":
    parser = argparse.ArgumentParser(description="Transforms text file given dictionaries into CNTK text format.")
    parser.add_argument('--map', help='Label mapping', required=True)
    parser.add_argument('--annotated', help='Whether to annotate indices with tokens. Default is false',
        choices=["True", "False"], default="False", required=False)
    parser.add_argument('--root', help='Root of the input and output file', default='.')
    parser.add_argument('--output', help='Name of the output file, stdout if not given', default="", required=False)
    parser.add_argument('--input', help='Name of the input file, stdin if not given', default="", required=False)
    parser.add_argument('--w2v', help='Word2Vec pre-trained model')
    args = parser.parse_args()

    # creating input
    input = [sys.stdin]
    if len(args.input) != 0:
        input = open(os.path.join(args.root, args.input), encoding='utf-8')

    # creating output
    output = sys.stdout
    if args.output != "":
        output = open(os.path.join(args.root, args.output), "w", encoding='utf-8')

    convert(args.w2v, open(args.map, encoding='utf-8'), input, output, args.annotated == "True")
    output.flush()
    if (output != sys.stdout):
        output.close()