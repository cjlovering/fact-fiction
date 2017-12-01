from __future__ import print_function
from __future__ import absolute_import
from __future__ import division
import os
import nltk
import torch

def init():
    import subprocess

    share_path = os.environ['AZUREML_NATIVE_SHARE_DIRECTORY'] if 'AZUREML_NATIVE_SHARE_DIRECTORY' in os.environ else 'outputs/'
    glove_path = 'GloVe/glove.840B.300d.txt'
    model_path = os.path.join(share_path, 'infersent.allnli.pickle')
    if not os.path.exists(glove_path):
        subprocess.call('./get_infersent_data.bash', shell=True)
    else:
        print('Found', glove_path)

    # download popular data, models
    nltk.download('punkt')
    
    global infersent
    infersent = torch.load(model_path, map_location=lambda storage, loc: storage)
    infersent.set_glove_path(glove_path)

    infersent.build_vocab_k_words(K=100000)


def run(sentences):
    import json
    global infersent
    embeddings = infersent.encode(sentences, tokenize=True)
    data = {
        'embeddings': embeddings.tolist()
    }
    return json.dumps(data)


def main():
  from azureml.api.schema.dataTypes import DataTypes
  from azureml.api.schema.sampleDefinition import SampleDefinition
  from azureml.api.realtime.services import generate_schema

  sentences = ['This is a sentence.', 'This is another one.']

  # Test the output of the functions
  init()
  result = run(sentences)
  
  inputs = {"sentences": SampleDefinition(DataTypes.STANDARD, sentences)}
  
  #Generate the schema
  generate_schema(run_func=run, inputs=inputs, filepath='infersent_service_schema.json')
  print("Schema generated")


if __name__ == "__main__":
    main()

