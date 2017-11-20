import { 
    RECEIVE_TEXT_ENTRY,
    RECEIVE_TOKENS,
    RECEIVE_FEED,
    RECEIVE_VOTES
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

export { receiveTokens, receiveTextEntry, receiveFeed, receiveVotes };