import { SHOW_DETAILS } from '../constants/actionTypes';

const showDetails = (id, show) => {
    return {
        type: SHOW_DETAILS,
        id,
        show
    }
};

export { showDetails };