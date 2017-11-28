import { 
    FETCHING_DETAILS,
    RECEIVE_DETAILS
} from '../constants/actionTypes';
import 'whatwg-fetch';
import { receiveDetails } from './receive'

const fetchingDetails = tokenId => ({
    type: FETCHING_DETAILS,
    tokenId
})

const fetchDetails = tokenId => {
    return (dispatch) => {
        dispatch(fetchingDetails(tokenId));
        return fetch(`/Sentences/Details/${tokenId}`, {
            method: "GET",
            credentials: 'same-origin'
        })
        .then(
            response => response.json(),
            error => console.log(`An error occured when fetching details of sentence with id ${tokenId}.`, error)
        )
        .then(json => dispatch(receiveDetails(tokenId, json)))
    }
}

export { fetchDetails };