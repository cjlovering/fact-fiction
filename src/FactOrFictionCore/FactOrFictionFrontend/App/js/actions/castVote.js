import 'whatwg-fetch';

import { RECEIVE_VOTE } from '../constants/actionTypes';
import { receiveTokens, receiveVotes } from './receive';

const castVote = (tokenId, type) => {
    const formData = new FormData();
    formData.append("sentenceId", tokenId);
    formData.append("type", type);
    return (dispatch) => {
        return fetch(`/Votes/Cast/`, {
            method: "POST",
            credentials: 'same-origin',
            body: formData
        })
        .then(
            response => response.json(),
            error => console.log(`An error occured when casting votes for sentence id: ${tokenId}. `, error)
        )
        .then(json => {
            dispatch(receiveTokens(json));
            dispatch(receiveVotes(json));  
        })
    }
}

export { castVote };