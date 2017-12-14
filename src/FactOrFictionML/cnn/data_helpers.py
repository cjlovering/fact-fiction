
import numpy as np
import re
import itertools
import pandas as pd
from collections import Counter


def load_data(data_file, classes):
    """
    Loads MR polarity data from files, splits the data into words and generates labels.
    Returns split sentences and labels.
    """
    data = pd.read_csv(data_file, sep='\t')
    x_text = data['sentence'].tolist()
    
    def to_onehot(i, n):
        return np.eye(n, dtype=int)[i].tolist()

    y = [to_onehot(classes.index(l), len(classes)) for l in data['label']]
    
    return [x_text, np.array(y)]


ADDITIONAL_TOKENS = ['<NUM>', '<TIME>', '<URL>', "'s", "''", "``", '<PAD>', '<UNK>']

def to_word2vec_indices(w2v_model, sent, max_num_word=60):
    vocab_size, embedding_size = w2v_model.wv.syn0.shape
    def to_index(word):
        if word in ADDITIONAL_TOKENS:
            return vocab_size + ADDITIONAL_TOKENS.index(word)
        if word in w2v_model.wv.vocab:
            return w2v_model.wv.vocab[word].index
        return vocab_size + ADDITIONAL_TOKENS.index('<UNK>')

    indices = [to_index(w) for w in sent.split()]
    if len(indices) < max_num_word:
        diff = max_num_word - len(indices)
        indices.extend([vocab_size + ADDITIONAL_TOKENS.index('<PAD>')] * diff)
    return indices[:max_num_word]


def add_additional_tokens(w2v_matrix):
    vocab_size, embedding_size = w2v_matrix.shape
    more = len(ADDITIONAL_TOKENS)
    w2v_matrix = np.block([[w2v_matrix, np.zeros((vocab_size, more))],
                           [np.zeros((more, embedding_size)), np.eye(more)]])
    return w2v_matrix


def batch_iter(data, batch_size, num_epochs, shuffle=True):
    """
    Generates a batch iterator for a dataset.
    """
    data = np.array(data)
    data_size = len(data)
    num_batches_per_epoch = int((len(data)-1)/batch_size) + 1
    for epoch in range(num_epochs):
        # Shuffle the data at each epoch
        if shuffle:
            shuffle_indices = np.random.permutation(np.arange(data_size))
            shuffled_data = data[shuffle_indices]
        else:
            shuffled_data = data
        for batch_num in range(num_batches_per_epoch):
            start_index = batch_num * batch_size
            end_index = min((batch_num + 1) * batch_size, data_size)
            yield shuffled_data[start_index:end_index]
