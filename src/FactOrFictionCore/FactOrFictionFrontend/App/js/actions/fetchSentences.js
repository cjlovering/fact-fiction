import { 
    FETCHING_SENTENCES,
    INVALIDATE_TEXT_ENTRY,
} from '../constants/actionTypes';

import 'whatwg-fetch';
import { selectEntry } from './selectEntry';
import { receiveFeed, receiveTextEntry, receiveSentences, receiveVotes } from './receive';

const fetchingSentences = text => {
    return {
        type: FETCHING_SENTENCES,
        text
    };
};

const fetchTextEntry = textEntry => {
    return (dispatch) => {

        // Notify App that async call is being made.
        dispatch(fetchingSentences(textEntry));
        
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
            dispatch(receiveSentences(json));
            dispatch(receiveTextEntry(json));
            dispatch(selectEntry(clearSelection));
        })
    }
}

const fetchFeedSentences = (sentenceId = "", page = 0) => {
    return (dispatch) => {
        return fetch(`/Sentences/Feed/${sentenceId}?page=${page}`, {
            method: "GET",
            credentials: "same-origin"
        })
        .then(
            response => response.json(),
            error => console.log('An error occured when fetching feed entry.', error)
        )
        .then(json => {
            // Put sentences into storage
            dispatch(receiveSentences(json));
            // Add sentences to feed list
            dispatch(receiveFeed(json));
            // Add votes to the vote list
            dispatch(receiveVotes(json));
        })
    }
}

export { fetchTextEntry, fetchFeedSentences };