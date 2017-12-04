import 'whatwg-fetch';

import { RECEIVE_VOTE } from '../constants/actionTypes';
import { receiveSentences, receiveVotes } from './receive';

const castVote = (sentenceId, type) => {
    const formData = new FormData();
    formData.append("sentenceId", sentenceId);
    formData.append("type", type);
    return (dispatch) => {
        return fetch(`/Votes/Cast/`, {
            method: "POST",
            credentials: 'same-origin',
            body: formData
        })
        .then(
            response => response.json(),
            error => console.log(`An error occured when casting votes for sentence id: ${sentenceId}. `, error)
        )
        .then(json => {
            dispatch(receiveSentences(json));
            dispatch(receiveVotes(json));  
        })
    }
}

export { castVote };