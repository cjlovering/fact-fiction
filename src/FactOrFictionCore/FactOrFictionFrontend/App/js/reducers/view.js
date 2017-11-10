import { VIEW_INPUT, VIEW_RESULT } from '../constants/viewTypes';
export default (state = VIEW_INPUT, view) => {
    switch(view.view) {
        case VIEW_INPUT:
        case VIEW_RESULT:
            return view.view;
        default:
            return state;
    }
}
