import { 
    CHANGE_VIEW, 
    SELECT_ENTRY,
    POST_TEXT_ENTRY,
    RECIEVE_TEXT_ENTRY,
    INVALIDATE_TEXT_ENTRY
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

const recieveTextEntryTokens = (text, json) => {
    return {
        type: RECIEVE_TEXT_ENTRY,
        text,
        textEntryTokens: json.sentences,
        receivedAt: Date.now()
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
            error => console.log('An error occured.', error)
        )
        .then(json => {
            // dispatch action when the response is recieved.
            dispatch(recieveTextEntryTokens(textEntry, json));
            dispatch(selectEntry());
        })
    }
}

export { changeView, selectEntry, postTextEntry, recieveTextEntryTokens, fetchTextEntry };
