import numpy as np
from gensim.models import KeyedVectors

ADDITIONAL_TOKEN = ['<NUM>', '<TIME>', '<URL>', "'s", "''", "``"]

def to_word_vector(w2v_model, token):
    word_vec = np.zeros(w2v_model.vector_size)
    additional_vec = np.zeros(len(ADDITIONAL_TOKEN) + 1)
    if token not in w2v_model.vocab:
        if token in ADDITIONAL_TOKEN:
            additional_vec[ADDITIONAL_TOKEN.index(token)] = 1
        else:
            additional_vec[-1] = 1
    else:
        word_vec = w2v_model.word_vec(token)

    return np.concatenate((word_vec, additional_vec))