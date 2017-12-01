import { 
    FETCHING_SIMILAR,
    RECEIVE_SIMILAR
} from '../constants/actionTypes';
import 'whatwg-fetch';
import { receiveSimilar, receiveTokens, receiveVotes } from './receive'

const fetchingSimilar = tokenId => {
    return {
        type: FETCHING_SIMILAR,
        tokenId
    }
}

const fetchSimilarTokens = (tokenId) => {
    return (dispatch) => {
        return fetch(`/Sentences/Related/${tokenId}`, {
            method: "GET",
            credentials: "same-origin"
        })
        .then(
            response => response.json()
        )
        .then(json => {
            // Put tokens into storage
            dispatch(receiveTokens(json));
            // Add votes to the vote list
            dispatch(receiveVotes(json));
            // Add similar sentences to the similar map
            dispatch(receiveSimilar(tokenId, json));
        })
        .catch(
            error => console.log('An error occured when fetching similar sentences.', error)
        )
    }
}

export { fetchingSimilar, fetchSimilarTokens };