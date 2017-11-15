import { CHANGE_VIEW } from '../constants/actionTypes';

const changeView = view => {
    return {
        type: CHANGE_VIEW,
        view
    };
};

export { changeView };