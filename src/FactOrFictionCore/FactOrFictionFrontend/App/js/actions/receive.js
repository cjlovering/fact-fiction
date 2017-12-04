import { 
    RECEIVE_TEXT_ENTRY,
    RECEIVE_SENTENCES,
    RECEIVE_FEED,
    RECEIVE_VOTES,
    RECEIVE_DETAILS,
    RECEIVE_SIMILAR
} from '../constants/actionTypes';

const receiveSentences = json => {
    return {
        type: RECEIVE_SENTENCES,
        sentences: json.sentences.reduce(
            (map, entry) => { map[entry.id] = entry; return map; }, {})
    };
};

const receiveTextEntry = json => {
    return {
        type: RECEIVE_TEXT_ENTRY,
        textEntrySentenceIds: json.sentences.map(entry => entry.id)
    };
};

const receiveFeed = json => {
    return {
        type: RECEIVE_FEED,
        feedSentenceIds: json.sentences.map(entry => entry.id)
    };
};

const receiveVotes = json => {
    return {
        type: RECEIVE_VOTES,
        votes: json.votes
    }
}

const receiveDetails = (sentenceId, json) => {
    let details = {}
    details[sentenceId] = {
        references: json.references,
        entities: json.entities
    }
    
    return {
        type: RECEIVE_DETAILS,
        details
    }
}

const receiveSimilar = (sentenceId, json) => {
    let similarSentenceIds = {}
    similarSentenceIds[sentenceId] = json.sentences.map(entry => entry.id)
    return {
        type: RECEIVE_SIMILAR,
        similarSentenceIds
    }
}

export { receiveSentences, receiveTextEntry, receiveFeed, receiveDetails, receiveVotes, receiveSimilar };