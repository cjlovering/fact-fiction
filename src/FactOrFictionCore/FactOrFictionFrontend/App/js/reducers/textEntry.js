import {
    FETCHING_SENTENCES
} from '../constants/actionTypes';

/* textEntry details */
export default (
    state = "",
    action
) => {
    switch(action.type) {
        case FETCHING_SENTENCES:
            return action.text;
        default:
            return state;
    }
}