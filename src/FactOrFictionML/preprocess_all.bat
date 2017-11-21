@echo off

echo Aggregate all raw data...
python preprocess/aggregate_raw_data.py ../../data
echo Generate MTusk tasks...
python preprocess/generate_mtusk_tasks.py ../../data
echo Generate a dictionary...
python preprocess/generate_dictionary.py ../../data/master_all.tsv sentence --output dictionary.txt

echo "Please open the Final_Cleaning_And_Splitting notebook and run all the cells."