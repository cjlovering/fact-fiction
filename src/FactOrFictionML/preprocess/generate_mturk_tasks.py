from __future__ import print_function

import random
import sys

random.seed(23)

datasets = [
    'fake',
    'bbc',
    'fox',
    'cnn'
]

task = []
path_prefix = sys.argv[1] if len(sys.argv) >= 2 else '.'
for d in datasets:
    path_format = path_prefix + "/{0}/all.tsv"
    with open(path_format.format(d), 'r', encoding='utf-8') as ds:
        data = ds.readlines()
        header = data[0]
        data = data[1:]
        sample = random.sample(data, 2500)
        task.extend(sample)

random.shuffle(task)
first_batch = task[:5000]
second_batch = task[5000:]

path_task_1 = path_prefix + "/mtusk_task_1_5000.tsv"
with open(path_task_1, 'w', encoding='utf-8') as f:
    f.write(header)
    f.write("".join(first_batch))

path_task_2 = path_prefix + "/mtusk_task_2_5000.tsv"
with open(path_task_2, 'w', encoding='utf-8') as f:
    f.write(header)
    f.write("".join(second_batch))

print('MTusk tasks created:')
print(path_task_1)
print(path_task_2)