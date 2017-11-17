# Data

Directory hierarchy after processing all raw data:

```
/data
    /bcc
        /raw
            /business
            /entertainment
            ...
        all.tsv
    /cnn
        /raw
            /cnn_crime
            /cnn_entertainment
            ...
        all.tsv
    ...
    /final
        final.tsv
        final.test.tsv
        final.train.tsv
        final.val.tsv
    mtusk_task_1_5000.tsv
    mtusk_task_2_5000.tsv
    master_all.tsv
    dictionary.txt
```


### bbc

http://mlg.ucd.ie/datasets/bbc.html

### cnn

https://sites.google.com/site/qianmingjie/home/datasets/cnn-and-fox-news

### fox

https://sites.google.com/site/qianmingjie/home/datasets/cnn-and-fox-news

### imdb

https://www.cs.cornell.edu/people/pabo/movie-review-data/

### How to preprocess data

After downloading and putting all raw data into the `data` directory, change the current directory to `src/FactOrFictionML` and do the following steps:

**Aggregate raw data across data source**

```
python preprocess/aggregate_raw_data.py data
```

After this step, a `all.tsv` file will be generated for each data source. A `master_all.tsv`, containing data across all data source, will also be generated in the `data` directory.

**Generate MTusk tasks (optional)**

```
python preprocess/generate_mtusk_tasks.py data
```

The tasks will be generated in the `data` directory.

**Generate a dictionary**

```
python preprocess/generate_dictionary.py data/master_all.tsv sentence --output data/dictionary.txt
```

A `dictionary.txt` will be generated in the `data` directory.

**Final cleaning and splitting dataset**

Open the `Final_Cleaning_And_Splitting.ipynb` notebook then run through all its cells.

All output will be put in the `data/final` directory.


**Convert final datasets to ctf files**

```
python preprocess/convert_to_ctf.py --root data --input final/final.tsv --output final/final.ctf --map data/dictionary.txt labels.txt --annotated True
```

Replace the arguments for `--input` and `--output` to convert other files in the `final` directory.

### Generated files

#### all.tsv

Contains the tokenized sentences across all categories for each data source.

#### master_all.tsv

Contains the tokenized sentences across all data source.

#### dictionary.txt

Contains all vocabulary across all data source.

#### mtusk_task_1_5000.tsv and mtusk_task_2_5000.tsv

All of the all.tsv are merged and shuffled. 10,000 examples are sampled from this almalgam and split into 2 tasks, ready for MTusk.

#### final*.tsv

Contains cleaned and splitted data.