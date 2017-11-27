USER="wpihacc"
HOST="fact-or-fiction.eastus.cloudapp.azure.com"
REMOTE_ROOT="~/.azureml/share/FactOrFictionMLExperiment/FactOrFictionWorkspace/FactOrFictionML/"

echo "Ensure directory exists"
ssh $USER@$HOST mkdir -p $REMOTE_ROOT/data

echo "Copy data to VM"
rsync -rv --progress ../../data/final $USER@$HOST:$REMOTE_ROOT/data
