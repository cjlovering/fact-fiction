import { ADD_FACT } from './actionTypes';

/* action creators */
const addFact = text => {
    return {
        type: ADD_FACT,
        text
    };
};

export { addFact };
