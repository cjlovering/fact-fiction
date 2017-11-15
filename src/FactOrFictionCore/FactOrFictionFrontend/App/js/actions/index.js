import { 
    CHANGE_VIEW, 
    SELECT_ENTRY,
    POST_TEXT_ENTRY,
    RECEIVE_TEXT_ENTRY,
    INVALIDATE_TEXT_ENTRY,
    RECEIVE_TOKENS,
    RECEIVE_FEED
} from '../constants/actionTypes';

import fetch from 'isomorphic-fetch';

/* action creators */
const changeView = view => {
    return {
        type: CHANGE_VIEW,
        view
    };
};

const selectEntry = id => {
    return {
        type: SELECT_ENTRY,
        id
    }
};

const postTextEntry = text => {
    return {
        type: POST_TEXT_ENTRY,
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
        dispatch(postTextEntry(textEntry));
        
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

const fetchFeedTokens = () => {
    return (dispatch) => {
        return fetch(`/Sentences/Feed/`, {
            method: "GET",
            credentials: "same-origin"
        })
        .then(
            response => response.json(),
            error => console.log('An error occured when fetching feed entry.', error)
        )
        .then(json => {
            const clearSelection = "";
            dispatch(receiveTokens(json));
            dispatch(receiveFeed(json));
            dispatch(selectEntry(clearSelection));
        })
    }
}

export { changeView, selectEntry, postTextEntry, receiveTokens, fetchTextEntry, fetchFeedTokens };