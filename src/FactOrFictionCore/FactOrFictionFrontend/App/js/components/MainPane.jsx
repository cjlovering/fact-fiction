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
    render() {
        const { view, fetchTextEntry, selectedEntryId, changeView, textEntryTokens, selectEntry } = this.props;
        const isInput = view === VIEW_INPUT;
        const title = isInput ? "Start" : "Results";
        const leftPane = isInput ? (
            // Input View
            <InputPane fetchTextEntry={fetchTextEntry} changeView={changeView} />
        ) : (
           <ResultPane 
                selectedEntryId={selectedEntryId} 
                selectEntry={selectEntry} 
                textEntryTokens={textEntryTokens} 
                changeView={changeView} 
            />
        );
        const rightPane = isInput ? (
            // TODO: render feed
            <div />
        ) : (
            // Result View
            <div>
                <ListView entries={textEntryTokens} selectedEntryId={selectedEntryId} selectEntry={selectEntry} />
            </div>
        );
        return (
            <div>
                <span className="ms-font-su ms-fontColor-themePrimary">{title}</span>
                <div className="container left-bar">
                    <div className="row">
                        <div className="col-sm-6 col-md-6 col-lg-6">
                            {leftPane}
                        </div>
                        <div className="col-sm-6 col-md-6 col-lg-6">
                            {rightPane}
                        </div>
                    </div>
                </div>
            </div>
        )
    }
}

MainPane.propTypes = {
    view: PropTypes.string.isRequired,
    changeView: PropTypes.func.isRequired,
    selectedEntryId: PropTypes.string.isRequired,
    fetchTextEntry: PropTypes.func.isRequired,
    textEntryTokens: PropTypes.array.isRequired,
    selectEntry: PropTypes.func.isRequired
}