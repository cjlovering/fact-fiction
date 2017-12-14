#! /usr/bin/env python
# Code taken from https://github.com/dennybritz/cnn-text-classification-tf

import tensorflow as tf
import numpy as np
import os
import time
import datetime
import data_helpers
from text_cnn import TextCNN
from tensorflow.contrib import learn
from gensim.models.keyedvectors import KeyedVectors
import csv

azureml_share_env = 'AZUREML_NATIVE_SHARE_DIRECTORY'
is_azure_ml = azureml_share_env in os.environ
share_path = os.environ[azureml_share_env] if is_azure_ml else '../../../'
test_path = os.path.join(share_path, "data/final/final.test.tsv")
w2v_path = os.path.join(share_path, "data/GoogleNews-vectors-negative300-SLIM.bin.gz")

# Parameters
# ==================================================

# Data Parameters
# tf.flags.DEFINE_string("data_file", "../../../data/final/final.tsv", "Data source.")

# Eval Parameters
tf.flags.DEFINE_integer("batch_size", 64, "Batch Size (default: 64)")
tf.flags.DEFINE_string("checkpoint_dir", "", "Checkpoint directory from training run")
tf.flags.DEFINE_boolean("eval_train", False, "Evaluate on all training data")

# Misc Parameters
tf.flags.DEFINE_boolean("allow_soft_placement", True, "Allow device soft device placement")
tf.flags.DEFINE_boolean("log_device_placement", False, "Log placement of ops on devices")
tf.flags.DEFINE_integer("max_sentence_length", 60, "Maximum sentence length")

FLAGS = tf.flags.FLAGS
FLAGS._parse_flags()
print("\nParameters:")
for attr, value in sorted(FLAGS.__flags.items()):
    print("{}={}".format(attr.upper(), value))
print("")

# Load Google word2vec
print("Loading word2vec...")
w2v_model = KeyedVectors.load_word2vec_format(w2v_path, binary=True)

# CHANGE THIS: Load data. Load your own data here
labels = ['objective', 'subjective']
if FLAGS.eval_train:
    x_raw, y_test = data_helpers.load_data(test_path, labels)
    y_test = np.argmax(y_test, axis=1)
else:
    x_raw = ["Donald Trump is the president", "I think this is a sentence", "The sky is blue", "The earth is flat"]
    y_test = [0, 1, 0, 0]

# Map data into vocabulary
# vocab_path = os.path.join(FLAGS.checkpoint_dir, "..", "vocab")
# vocab_processor = learn.preprocessing.VocabularyProcessor.restore(vocab_path)
# x_test = np.array(list(vocab_processor.transform(x_raw)))

x_test = np.array([data_helpers.to_word2vec_indices(w2v_model, sent, FLAGS.max_sentence_length) for sent in x_raw])

print("\nEvaluating...\n")

# Evaluation
# ==================================================
checkpoint_file = tf.train.latest_checkpoint(FLAGS.checkpoint_dir)
graph = tf.Graph()
with graph.as_default():
    session_conf = tf.ConfigProto(
      allow_soft_placement=FLAGS.allow_soft_placement,
      log_device_placement=FLAGS.log_device_placement)
    sess = tf.Session(config=session_conf)
    with sess.as_default():
        # Load the saved meta graph and restore variables
        saver = tf.train.import_meta_graph("{}.meta".format(checkpoint_file))
        saver.restore(sess, checkpoint_file)

        # Get the placeholders from the graph by name
        input_x = graph.get_operation_by_name("input_x").outputs[0]
        # input_y = graph.get_operation_by_name("input_y").outputs[0]
        dropout_keep_prob = graph.get_operation_by_name("dropout_keep_prob").outputs[0]

        # Tensors we want to evaluate
        predictions = graph.get_operation_by_name("output/predictions").outputs[0]

        # Generate batches for one epoch
        batches = data_helpers.batch_iter(list(x_test), FLAGS.batch_size, 1, shuffle=False)

        # Collect the predictions here
        all_predictions = []

        for x_test_batch in batches:
            batch_predictions = sess.run(predictions, {input_x: x_test_batch, dropout_keep_prob: 1.0})
            all_predictions = np.concatenate([all_predictions, batch_predictions])

# Print accuracy if y_test is defined
if y_test is not None:
    correct_predictions = float(sum(all_predictions == y_test))
    print("Total number of test examples: {}".format(len(y_test)))
    print("Accuracy: {:g}".format(correct_predictions/float(len(y_test))))

# Save the evaluation to a csv
predictions_human_readable = np.column_stack((np.array(x_raw), all_predictions))
out_path = os.path.join(FLAGS.checkpoint_dir, "..", "prediction.csv")
print("Saving evaluation to {0}".format(out_path))
with open(out_path, 'w') as f:
    csv.writer(f).writerows(predictions_human_readable)