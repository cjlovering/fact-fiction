import { 
    FETCHING_SIMILAR,
    RECEIVE_SIMILAR
} from '../constants/actionTypes';
import 'whatwg-fetch';
import { receiveSimilar, receiveSentences, receiveVotes } from './receive'

const fetchingSimilar = sentenceId => {
    return {
        type: FETCHING_SIMILAR,
        sentenceId
    }
}

const fetchSimilarSentences = (sentenceId) => {
    return (dispatch) => {

        // Notify App that async call is being made.
         dispatch(fetchingSimilar(sentenceId));

        return fetch(`/Sentences/Related/${sentenceId}`, {
            method: "GET",
            credentials: "same-origin"
        })
        .then(
            response => response.json()
        )
        .then(json => {
            // Put sentences into storage
            dispatch(receiveSentences(json));
            // Add votes to the vote list
            dispatch(receiveVotes(json));
            // Add similar sentences to the similar map
            dispatch(receiveSimilar(sentenceId, json));
        })
        .catch(
            error => console.log('An error occured when fetching similar sentences.', error)
        )
    }
}

export { fetchingSimilar, fetchSimilarSentences };