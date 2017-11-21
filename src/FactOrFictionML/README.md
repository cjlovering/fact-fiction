# Fact or Fiction Machine Learning Service

## How to preprocess data

The following steps can be quickly run in batch by using the `preprocess_all.bat` and `convert_to_ctf_all.bat`.

After downloading and putting all raw data into the `data` directory, do the following steps:

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

All output will be put in the `../../data/final` directory.


**Convert final datasets to ctf files**

```
python preprocess/convert_to_ctf.py --root data --input final/final.tsv --output final/final.ctf --map dictionary.txt labels.txt --annotated True
```

Replace the arguments for `--input` and `--output` to convert other files in the `final` directory.

## Generated files

### all.tsv

Contains the tokenized sentences across all categories for each data source.

### master_all.tsv

Contains the tokenized sentences across all data source.

### dictionary.txt

Contains all vocabulary across all data source.

### mtusk_task_1_5000.tsv and mtusk_task_2_5000.tsv

All of the all.tsv are merged and shuffled. 10,000 examples are sampled from this almalgam and split into 2 tasks, ready for MTusk.

### final*.tsv

Contains cleaned and splitted data.

## Training the model

After generate all data files, copy the `final` folder to `~/azuremlshared/FactOrFictionMLExperiment/FactOrFictionWorkspace/FactOrFictionML/data`

Open the FactOrFictionML project with Azure ML Workbench and start training the model.

## Deploying the model locally

First, install [Docker](https://www.docker.com/get-docker).

From Azure Machine Learning Workbench, go to File > Open Command Prompt then run the following commands.

```
az provider register -n Microsoft.MachineLearningCompute
az provider register -n Microsoft.ContainerRegistry
az ml env set -g FactOrFiction -n localmlserviceenv
az ml account modelmanagement set -n FactOrFictionMLExperimentModelMgmt -g FactOrFiction
```

The `az provider register` commands take some time to finish.

If `az env set` or `az modelmanagement set` cannot find the environment or account, create them by following [this instruction](https://docs.microsoft.com/en-us/azure/machine-learning/preview/deployment-setup-configuration).

Run score.py to generate `service_schema.json` in the `outputs` directory. If you run with Azure Machine Learning Workbench, make sure to download the generated `service_schema.json` and put it in the local `outputs` directory. Also change `lstm_model.cmf` to the name of the model being used, this model must also exist the `outputs` directory. After that, run the following commands.

```
az ml image create -r python -f score.py -s outputs\service_schema.json -m outputs\lstm_model.cmf -d preprocess -d dictionary.txt -d labels.txt -n factorfictionmlserviceimg -c aml_config\conda_dependencies.yml
```

This command may take a couple minutes. It then gives us an ID of the created service image. Use this ID in the following command to start the service.

```
az ml service create realtime --image-id <img-id> -n factorfictionmlservice
```