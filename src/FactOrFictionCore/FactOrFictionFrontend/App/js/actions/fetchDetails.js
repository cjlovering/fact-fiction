import { 
    FETCHING_DETAILS,
    RECEIVE_DETAILS
} from '../constants/actionTypes';
import 'whatwg-fetch';
import { receiveDetails } from './receive'

const fetchingDetails = sentenceId => ({
    type: FETCHING_DETAILS,
    sentenceId
})

const fetchDetails = sentenceId => {
    return (dispatch) => {
        dispatch(fetchingDetails(sentenceId));
        return fetch(`/Sentences/Details/${sentenceId}`, {
            method: "GET",
            credentials: 'same-origin'
        })
        .then(
            response => response.json(),
            error => console.log(`An error occured when fetching details of sentence with id ${sentenceId}.`, error)
        )
        .then(json => dispatch(receiveDetails(sentenceId, json)))
    }
}

export { fetchDetails };