import { ADD_FACT, CHANGE_VIEW } from '../constants/actionTypes';

/* action creators */
const addFact = text => {
    return {
        type: ADD_FACT,
        text
    };
};

const changeView = view => {
    return {
        type: CHANGE_VIEW,
        view
    };
};

export { addFact, changeView };
