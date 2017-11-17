@echo off

echo final.tsv
python preprocess/convert_to_ctf.py --root data --input final/final.tsv --output final/final.ctf --map data/dictionary.txt labels.txt --annotated True
echo final.train.tsv
python preprocess/convert_to_ctf.py --root data --input final/final.train.tsv --output final/final.train.ctf --map data/dictionary.txt labels.txt --annotated True
echo final.val.tsv
python preprocess/convert_to_ctf.py --root data --input final/final.val.tsv --output final/final.val.ctf --map data/dictionary.txt labels.txt --annotated True
echo final.test.tsv
python preprocess/convert_to_ctf.py --root data --input final/final.test.tsv --output final/final.test.ctf --map data/dictionary.txt labels.txt --annotated True

echo Done