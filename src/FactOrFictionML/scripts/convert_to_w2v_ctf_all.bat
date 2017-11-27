echo Convert using word2vec
echo final.tsv
python preprocess/convert_to_w2v_ctf.py --root ../../data --input final/final.tsv --output final/final.w2v.ctf --map labels.txt --w2v ../../data/GoogleNews-vectors-negative300.bin.gz --annotated True
echo final.train.tsv
python preprocess/convert_to_w2v_ctf.py --root ../../data --input final/final.train.tsv --output final/final.train.w2v.ctf --map labels.txt --w2v ../../data/GoogleNews-vectors-negative300.bin.gz --annotated True
echo final.val.tsv
python preprocess/convert_to_w2v_ctf.py --root ../../data --input final/final.val.tsv --output final/final.val.w2v.ctf --map labels.txt --w2v ../../data/GoogleNews-vectors-negative300.bin.gz --annotated True
echo final.test.tsv
python preprocess/convert_to_w2v_ctf.py --root ../../data --input final/final.test.tsv --output final/final.test.w2v.ctf --map labels.txt --w2v ../../data/GoogleNews-vectors-negative300.bin.gz --annotated True

echo Done