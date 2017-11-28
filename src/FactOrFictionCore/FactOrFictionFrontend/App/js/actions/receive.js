import { 
    RECEIVE_TEXT_ENTRY,
    RECEIVE_TOKENS,
    RECEIVE_FEED,
    RECEIVE_VOTES,
    RECEIVE_DETAILS,
    RECEIVE_SIMILAR
} from '../constants/actionTypes';

const receiveTokens = json => {
    return {
        type: RECEIVE_TOKENS,
        tokens: json.sentences.reduce(
            (map, entry) => { map[entry.id] = entry; return map; }, {})
    };
};

const receiveTextEntry = json => {
    return {
        type: RECEIVE_TEXT_ENTRY,
        textEntryTokenIds: json.sentences.map(entry => entry.id)
    };
};

const receiveFeed = json => {
    return {
        type: RECEIVE_FEED,
        feedTokenIds: json.sentences.map(entry => entry.id)
    };
};

const receiveVotes = json => {
    return {
        type: RECEIVE_VOTES,
        votes: json.votes
    }
}

const receiveDetails = (tokenId, json) => {
    let details = {}
    details[tokenId] = {
        references: json.references,
        entities: json.entities
    }
    
    return {
        type: RECEIVE_DETAILS,
        details
    }
}

const receiveSimilar = (tokenId, json) => {
    let similarTokenIds = {}
    similarTokenIds[tokenId] = json.sentences.map(entry => entry.id)
    return {
        type: RECEIVE_SIMILAR,
        similarTokenIds
    }
}

export { receiveTokens, receiveTextEntry, receiveFeed, receiveDetails, receiveVotes, receiveSimilar };