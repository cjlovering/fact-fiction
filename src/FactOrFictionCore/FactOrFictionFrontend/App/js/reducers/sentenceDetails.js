import { 
    FETCHING_DETAILS,
    RECEIVE_DETAILS
} from '../constants/actionTypes';

/* text entry */
export default (
    state = {
        isFetching: false,
        details: {}, // token id --> { entities: [], references: [] }
    },
    action
) => {
    switch(action.type) {
        case FETCHING_DETAILS:
            return {...state, isFetching: true}
        case RECEIVE_DETAILS:
            const details = { ...state.details, ...action.details}
            const isFetching = false;
            return { isFetching, details };
        default:
            return state;
    }
}