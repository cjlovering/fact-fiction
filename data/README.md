# Data

Directory hierarchy after processing all raw data

```
/data
    /bcc
        /raw
            /business
            /entertainment
            ...
        all.csv
    /cnn
        /raw
            /cnn_crime
            /cnn_entertainment
            ...
        all.csv
    ...
    mtusk_task_1_5000.csv
    mtusk_task_2_5000.csv
    master_all.csv
```


### bbc

http://mlg.ucd.ie/datasets/bbc.html

### cnn

https://sites.google.com/site/qianmingjie/home/datasets/cnn-and-fox-news

### fox

https://sites.google.com/site/qianmingjie/home/datasets/cnn-and-fox-news

### imdb

https://www.cs.cornell.edu/people/pabo/movie-review-data/

### generated

#### all.csv

Contains the tokenized sentences across all categories for each news source.

#### master_all.csv

Contains the tokenized sentences across all news source.

#### mtusk_task_1_5000.csv and mtusk_task_2_5000.csv

All of the all.csv are merged and shuffled. 10,000 examples are sampled from this almalgam and split into 2 tasks, ready for MTusk.