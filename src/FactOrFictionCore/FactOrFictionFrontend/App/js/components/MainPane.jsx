import React from 'react'
import PropTypes from 'prop-types';

import InputPane from './InputPane';
import ListView from './ListView';
import Button from './Button';
import Sentence from './Sentence';
import ResultPane from './ResultPane';

import { VIEW_INPUT } from '../constants/viewTypes';
import _ from '../../stylesheets/components/_MainPane.scss';

export default class MainPane extends React.Component {
    componentDidMount() {
        const { fetchFeedTokens } = this.props;
        fetchFeedTokens();
    }

    render() {
        const {
            tokens, isFetching, didInvalidate, textEntryTokenIds, feedTokenIds,
            view, selectedEntryId, selectEntry, fetchTextEntry, changeView
        } = this.props;
        const isInput = view === VIEW_INPUT;
        const title = isInput ? "Input" : "Results";
        const leftPane = isInput ? (
            <InputPane fetchTextEntry={fetchTextEntry} changeView={changeView} />
        ) : (
           <ResultPane 
                selectedEntryId={selectedEntryId} 
                selectEntry={selectEntry} 
                textEntryTokens={textEntryTokenIds.map(id => tokens[id])} 
                changeView={changeView} 
            />
        );
        const entries = (isInput ? feedTokenIds : textEntryTokenIds)
            .map(id => tokens[id]);
        return (
            <div>
                <div className="container">
                    <span className="ms-font-su ms-fontColor-themePrimary">{title}</span>
                    <div className="row">
                        <div className="col-sm-6 col-md-6 col-lg-6">
                            {leftPane}
                        </div>
                        <div className="col-sm-6 col-md-6 col-lg-6">
                            <ListView
                                entries={entries}
                                selectedEntryId={selectedEntryId}
                                selectEntry={selectEntry}
                            />
                        </div>
                    </div>
                </div>
            </div>
        )
    }
}

MainPane.propTypes = {
    tokens: PropTypes.object.isRequired,
    isFetching: PropTypes.bool.isRequired,
    didInvalidate: PropTypes.bool.isRequired,
    textEntryTokenIds: PropTypes.array.isRequired,
    feedTokenIds: PropTypes.array.isRequired,
    view: PropTypes.string.isRequired,
    selectedEntryId: PropTypes.string.isRequired,
    fetchTextEntry: PropTypes.func.isRequired,
    selectEntry: PropTypes.func.isRequired,
    fetchFeedTokens: PropTypes.func.isRequired,
    changeView: PropTypes.func.isRequired
}