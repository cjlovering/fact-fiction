import os
import cntk
import nltk

from preprocess.normalize_sentences import SentenceNormalizer

def init():
    # download popular data, models
    nltk.download('popular')

    from cntk.ops.functions import load_model
    # load the model and dictionary file
    share_path = os.environ['AZUREML_NATIVE_SHARE_DIRECTORY'] if 'AZUREML_NATIVE_SHARE_DIRECTORY' in os.environ else 'outputs/'
    
    global model
    model = load_model(os.path.join(share_path, 'lstm_model.cmf'))

    global dictionary
    with open('dictionary.txt', 'r', encoding='utf-8') as f:
        dictionary = f.read().strip().split('\n') 

    global vocab_size
    vocab_size = len(dictionary)

    global sent_normalizer
    sent_normalizer = SentenceNormalizer(dictionary=dictionary)

    global labels
    with open('labels.txt', 'r') as f:
        labels = f.read().strip().split('\n')


def run(text_entry):
    import json

    sentences = nltk.sent_tokenize(text_entry)
    normalized = sent_normalizer.fit_transform(sentences, to_index=True)

    pred = model(cntk.Value.one_hot(normalized, vocab_size))

    pred = pred.argmax(axis=1)
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

  text_entry = 'This is a sentence. This is another one.'

  # Test the output of the functions
  init()
  print("Result: " + run(text_entry))
  
  inputs = {"text_entry": SampleDefinition(DataTypes.STANDARD, text_entry)}
  
  #Generate the schema
  generate_schema(run_func=run, inputs=inputs, filepath='./outputs/service_schema.json')
  print("Schema generated")


if __name__ == "__main__":
    main()

