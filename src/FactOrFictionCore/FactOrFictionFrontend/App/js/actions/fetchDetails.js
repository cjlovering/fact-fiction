import { 
    FETCHING_DETAILS,
    RECEIVE_DETAILS
} from '../constants/actionTypes';
import 'whatwg-fetch';

const fetchingDetails = tokenId => ({
    type: FETCHING_DETAILS,
    tokenId
})

const recieveDetails = (tokenId, json) => {
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

const fetchDetails = tokenId => {
    return (dispatch) => {
        dispatch(fetchingDetails(tokenId));
        return fetch(`/Sentences/Details/${tokenId}`, {
            method: "GET",
            credentials: 'same-origin'
        })
        .then(
            response => response.json(),
            error => console.log('An error occured when fetching text entries.', error)
        )
        .then(json => dispatch(recieveDetails(tokenId, json)))
    }
}

export { fetchDetails };