import React from 'react';
import PropTypes from 'prop-types';
import shortid from 'shortid';
import FactCard from './FactCard';
import _ from '../../stylesheets/components/_ListView.scss';
import InfiniteScroll from 'react-infinite-scroller';
import {
    Spinner,
    SpinnerSize
} from 'office-ui-fabric-react/lib/Spinner';
import { VOTE_TRUE, VOTE_FALSE, VOTE_UNVOTED } from '../constants/voteTypes.js'

export default class ListView extends React.Component {
    static propTypes = {
        details: PropTypes.object.isRequired,
        fetchDetails: PropTypes.func.isRequired,
        entries: PropTypes.array.isRequired,
        votes: PropTypes.object.isRequired,
        selectedEntryId: PropTypes.string.isRequired,
        votes: PropTypes.object.isRequired,
        selectEntry: PropTypes.func.isRequired,
        loadFunc: PropTypes.func.isRequired,
        hasMore: PropTypes.bool.isRequired,
        detailsShown: PropTypes.object.isRequired,    
        showDetails: PropTypes.func.isRequired,
        castVote: PropTypes.func.isRequired
    }

    render() {
        const { 
            entries, 
            selectedEntryId, 
            hasMore,
            details,
            votes,
            selectEntry, 
            loadFunc, 
            detailsShown,
            fetchDetails, 
            showDetails,
            castVote
        } = this.props;
        const showingDetails = id =>
            detailsShown.hasOwnProperty(id) && detailsShown[id];
        const sentenceVote = id =>
            votes.hasOwnProperty(id) ? votes[id] : VOTE_UNVOTED;
        return (
            <div className='list-view'>
                <InfiniteScroll
                    pageStart={0}
                    loadMore={loadFunc}
                    hasMore={hasMore}
                    threshold={50}
                    loader={<div className="spinner"><Spinner size={SpinnerSize.large} /></div>}
                    useWindow={false}
                >
                    {
                        entries
                            .filter(entry => entry.type == "OBJECTIVE")
                            .map(entry => (
                                <FactCard
                                    {...entry}
                                    details={entry.id in details ? details[entry.id] : {}}
                                    fetchDetails={fetchDetails}
                                    selectedEntryId={selectedEntryId}
                                    selectEntry={selectEntry}
                                    showDetails={showDetails}
                                    showingDetails={showingDetails(entry.id)}
                                    castVote={castVote}
                                    sentenceVote={sentenceVote(entry.id)}
                                    key={entry.id}
                                />
                            )
                        )
                    }
                </InfiniteScroll>
            </div>                      
        );
    }
}
