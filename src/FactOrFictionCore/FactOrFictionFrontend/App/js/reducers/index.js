import { combineReducers } from 'redux'
import facts from './facts'
import view from './view'

const root = combineReducers({
  view,
  facts
})

export default root;
