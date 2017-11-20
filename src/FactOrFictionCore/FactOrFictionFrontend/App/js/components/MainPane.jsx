import React from 'react'
import PropTypes from 'prop-types';
import {
    Spinner,
    SpinnerSize
} from 'office-ui-fabric-react/lib/Spinner';

import InputPane from './InputPane';
import ListView from './ListView';
import Button from './Button';
import Sentence from './Sentence';
import ResultPane from './ResultPane';

import { VIEW_INPUT } from '../constants/viewTypes';
import _ from '../../stylesheets/components/_MainPane.scss';
import { fetchDetails } from '../actions/fetchDetails';

export default class MainPane extends React.Component {
    render() {
        const {
            tokens, 
            isFetching, 
            isDoneFetchingFeed, 
            didInvalidate, 
            textEntryTokenIds, 
            feedTokenIds, 
            view,
            selectedEntryId, 
            selectEntry, 
            votes,
            fetchTextEntry, 
            changeView, 
            fetchFeedTokens,
            details,
            fetchDetails,
            detailsShown,
            showDetails,
            castVote
        } = this.props;
        const isInput = view === VIEW_INPUT;
        const title = isInput ? "Input" : "Results";
        const leftPane = isInput ? (
            <InputPane
                fetchTextEntry={fetchTextEntry}
                changeView={changeView}
            />
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
        const loadFunc = isInput
            ? (page) => {
                fetchFeedTokens(feedTokenIds[0], page);
            }
            : () => {};
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
                                details={details}
                                fetchDetails={fetchDetails}
                                detailsShown={detailsShown}
                                showDetails={showDetails}
                                entries={entries}
                                selectedEntryId={selectedEntryId}
                                selectEntry={selectEntry}
                                loadFunc={loadFunc}
                                hasMore={isInput && !isDoneFetchingFeed}
                                castVote={castVote}
                                votes={votes}
                            />
                        </div>
                    </div>
                </div>
            </div>
        )
    }
}

MainPane.propTypes = {
    details: PropTypes.object.isRequired,
    fetchDetails: PropTypes.func.isRequired,
    tokens: PropTypes.object.isRequired,
    isFetching: PropTypes.bool.isRequired,
    didInvalidate: PropTypes.bool.isRequired,
    textEntryTokenIds: PropTypes.array.isRequired,
    feedTokenIds: PropTypes.array.isRequired,
    view: PropTypes.string.isRequired,
    selectedEntryId: PropTypes.string.isRequired,
    votes: PropTypes.object.isRequired,
    fetchTextEntry: PropTypes.func.isRequired,
    selectEntry: PropTypes.func.isRequired,
    fetchFeedTokens: PropTypes.func.isRequired,
    changeView: PropTypes.func.isRequired,
    showDetails: PropTypes.func.isRequired,
    detailsShown: PropTypes.object.isRequired,
    castVote: PropTypes.func.isRequired
}