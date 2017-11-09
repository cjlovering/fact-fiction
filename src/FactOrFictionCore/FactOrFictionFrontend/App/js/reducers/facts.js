import { ADD_FACT } from '../constants/actionTypes';

export default (state = [], text) => {
    switch (text.type) {
        case ADD_FACT:
            return [...state, text.text];
        default:
            return state;
    }
}
