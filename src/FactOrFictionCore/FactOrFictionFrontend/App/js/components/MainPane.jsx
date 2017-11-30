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

export default class MainPane extends React.Component {
    static propTypes = {
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
        castVote: PropTypes.func.isRequired,
        fetchSimilarTokens: PropTypes.func.isRequired,
        similarTokenIds: PropTypes.object.isRequired
    }
    
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
            castVote,
            fetchSimilarTokens,
            similarTokenIds
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
        const entries = (
            isInput 
            ? feedTokenIds 
            : textEntryTokenIds
        ).map(id => tokens[id]);

        const similarEntries = (
            similarTokenIds.hasOwnProperty(selectedEntryId) 
            ? similarTokenIds[selectedEntryId] 
            : []
        ).map(id => tokens[id]);
        const loadFunc = isInput
            ? (page) => {
                fetchFeedTokens(feedTokenIds[0], page);
            }
            : () => {};
        const handleClick = () => {

        }
        return (
            <div className="row"> 
                <div className="col-sm-12 col-md-12 col-lg-4 col-xl-5">
                    <span className="box-title ms-font-xxl ms-fontColor-themePrimary">{title}</span>
                    {leftPane}
                </div>
                <div className="col-sm-12 col-md-6 col-lg-4 col-xl-3-5">
                    <span className="list-title ms-font-xxl ms-fontColor-themePrimary">Objective Statements</span>
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
                        fetchSimilarTokens={fetchSimilarTokens}
                        similarTokenIds={similarTokenIds}
                    />
                </div>
                <div className="col-sm-12 col-md-6 col-lg-4 col-xl-3-5">
                    <span className="list-title ms-font-xxl ms-fontColor-themePrimary">Similar Sentences</span>
                    <ListView
                        details={details}
                        fetchDetails={fetchDetails}
                        detailsShown={detailsShown}
                        showDetails={showDetails}
                        entries={similarEntries}
                        selectedEntryId={selectedEntryId}
                        selectEntry={() => {}}
                        loadFunc={() => {}}
                        hasMore={false}
                        castVote={castVote}
                        votes={votes}
                        fetchSimilarTokens={fetchSimilarTokens}
                        similarTokenIds={similarTokenIds}
                    />
                </div>
            </div>
        )
    }
}
