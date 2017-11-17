import { 
    FETCHING_TOKENS,
    RECEIVE_TEXT_ENTRY,
    INVALIDATE_TEXT_ENTRY,
    RECEIVE_TOKENS,
    RECEIVE_FEED
} from '../constants/actionTypes';

import 'whatwg-fetch';
import { selectEntry } from './selectEntry';

const fetchingTokens = text => {
    return {
        type: FETCHING_TOKENS,
        text
    };
};

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

const fetchTextEntry = textEntry => {
    return (dispatch) => {

        // Notify App that async call is being made.
        dispatch(fetchingTokens(textEntry));
        
        // Construct form data that API is expecting.
        const formData = new FormData();
        formData.append("Content", textEntry);

        return fetch(`/TextEntries/Create/`, {
            method: "POST",
            credentials: 'same-origin',
            body: formData
        })
        .then(
            response => response.json(),
            error => console.log('An error occured when fetching text entries.', error)
        )
        .then(json => {
            const clearSelection = "";
            dispatch(receiveTokens(json));
            dispatch(receiveTextEntry(json));
            dispatch(selectEntry(clearSelection));
        })
    }
}

const fetchFeedTokens = (tokenId = "", page = 0) => {
    return (dispatch) => {
        return fetch(`/Sentences/Feed/${tokenId}?page=${page}`, {
            method: "GET",
            credentials: "same-origin"
        })
        .then(
            response => response.json(),
            error => console.log('An error occured when fetching feed entry.', error)
        )
        .then(json => {
            const clearSelection = "";
            // Put tokens into storage
            dispatch(receiveTokens(json));
            // Add tokens to feed list
            dispatch(receiveFeed(json));
            // Clear selection
            dispatch(selectEntry(clearSelection));
        })
    }
}

export { fetchTextEntry, fetchFeedTokens };