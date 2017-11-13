import {
    SELECT_ENTRY
} from '../constants/actionTypes';

export default (state = "", action) => {
    switch (action.type) {
        case SELECT_ENTRY:
            return (state === action.id || action.id === null)
                ? "" : action.id;
        default:
            return state;
    }
}