import { SELECT_ENTRY } from '../constants/actionTypes';

const selectEntry = id => {
    return {
        type: SELECT_ENTRY,
        id
    }
};

export { selectEntry };