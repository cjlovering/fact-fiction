import pandas as pd
import numpy as np
import json

df = pd.read_csv('final_unormalized.tsv', sep='\t')
df.columns = ['text', 'intent']
df['intent'] = df['intent'].map({'subjective': 'SuggestedOpinion', 'objective': 'SuggestedFact'})
df['entities'] = 'abc'
df['entities'] = df['entities'].map({'abc': []})
df = df[df['text'].apply(len) < 500] # LUIS wants texts with less than 500 characters 
df[:1000].to_json('luis_test.json', orient='records') # LUIS accepts at most 1000 entries per test file
