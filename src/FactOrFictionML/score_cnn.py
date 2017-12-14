import os
import tensorflow as tf
import numpy as np
import nltk
from gensim.models.keyedvectors import KeyedVectors
from cnn.data_helpers import to_word2vec_indices
from preprocess.normalize_sentences import SentenceNormalizer

def init():
    # download popular data, models
    nltk.download('punkt')

    # load the model and dictionary file
    share_path = os.environ['AZUREML_NATIVE_SHARE_DIRECTORY'] if 'AZUREML_NATIVE_SHARE_DIRECTORY' in os.environ else 'outputs/'
    w2v_path = os.path.join(share_path, "GoogleNewsW2V.SLIM.bin.gz")
    checkpoint_file = os.path.join(share_path, "cnn_model")

    global w2v_model
    w2v_model = KeyedVectors.load_word2vec_format(w2v_path, binary=True)

    global sent_normalizer
    sent_normalizer = SentenceNormalizer(discarded_tokens=['[', ']'])

    global graph
    graph = tf.Graph()
    with graph.as_default():

        global sess
        sess = tf.Session()
        with sess.as_default():
            #checkpoint_file = tf.train.latest_checkpoint(checkpoint_dir)
            # Load the saved meta graph and restore variables
            saver = tf.train.import_meta_graph("{}.meta".format(checkpoint_file))
            saver.restore(sess, checkpoint_file)

            global input_x
            global dropout_keep_prob
            global predictions
            # Get the placeholders from the graph by name
            input_x = graph.get_operation_by_name("input_x").outputs[0]
            # input_y = graph.get_operation_by_name("input_y").outputs[0]
            dropout_keep_prob = graph.get_operation_by_name("dropout_keep_prob").outputs[0]
            # Tensors we want to evaluate
            predictions = graph.get_operation_by_name("output/predictions").outputs[0]


def run(text_entry):
    import json
    global w2v_model
    global graph
    global sess
    global input_x
    global dropout_keep_prob
    global predictions
    labels = ['objective', 'subjective']

    sentences = nltk.sent_tokenize(text_entry)
    normalized = sent_normalizer.fit_transform(sentences)
    # TODO: should not hard-code the max length of a sentence
    normalized = np.array([to_word2vec_indices(w2v_model, sent, 60) for sent in normalized])

    pred = sess.run(predictions, {input_x: normalized, dropout_keep_prob: 1.0})
    pred = [labels[p] for p in pred]

    data = {
        'sentences': sentences,
        'pred': pred
    }
    return json.dumps(data)


def main():
  from azureml.api.schema.dataTypes import DataTypes
  from azureml.api.schema.sampleDefinition import SampleDefinition
  from azureml.api.realtime.services import generate_schema
  import numpy as np

  text_entry = 'This is a sentence. This is an awesome sentence!'

  # Test the output of the functions
  init()
  print("Result: " + run(text_entry))
  
  inputs = {"text_entry": SampleDefinition(DataTypes.STANDARD, text_entry)}
  
  #Generate the schema
  generate_schema(run_func=run, inputs=inputs, filepath='./outputs/cnn_service_schema.json')
  print("Schema generated")


if __name__ == "__main__":
    main()

