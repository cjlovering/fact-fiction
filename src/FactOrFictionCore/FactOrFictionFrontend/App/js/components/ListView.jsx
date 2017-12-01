import React from 'react';
import ReactDOM from 'react-dom';
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
        isMiddlePane: PropTypes.bool.isRequired,
        details: PropTypes.object.isRequired,
        fetchDetails: PropTypes.func.isRequired,
        entries: PropTypes.array.isRequired,
        votes: PropTypes.object.isRequired,
        selectedEntryId: PropTypes.string.isRequired,
        selectEntry: PropTypes.func.isRequired,
        loadFunc: PropTypes.func.isRequired,
        hasMore: PropTypes.bool.isRequired,
        detailsShown: PropTypes.object.isRequired,    
        showDetails: PropTypes.func.isRequired,
        castVote: PropTypes.func.isRequired,
        fetchSimilarTokens: PropTypes.func.isRequired,
        similarTokenIds: PropTypes.object.isRequired
    }

    render() {
        const { 
            isMiddlePane,
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
            castVote,
            fetchSimilarTokens,
            similarTokenIds
        } = this.props;
        const showingDetails = id =>
            detailsShown.hasOwnProperty(id) && detailsShown[id];
        const sentenceVote = id =>
            votes.hasOwnProperty(id) ? votes[id] : VOTE_UNVOTED;
        const domId = isMiddlePane ? 'list-view' : 'similar-view';
        return (
            <div className='list-view' id={domId}>
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
                                    key={entry.id}
                                    isInMiddlePane={isMiddlePane}
                                    {...entry}
                                    details={entry.id in details ? details[entry.id] : {}}
                                    fetchDetails={fetchDetails}
                                    selectedEntryId={selectedEntryId}
                                    selectEntry={selectEntry}
                                    showDetails={showDetails}
                                    showingDetails={showingDetails(entry.id)}
                                    castVote={castVote}
                                    sentenceVote={sentenceVote(entry.id)}
                                    fetchSimilarTokens={fetchSimilarTokens}
                                    similarTokenIds={similarTokenIds}
                                    ref={(card) => { if (entry.id === selectedEntryId) this.selectFactCard = card}}
                                />
                            )
                        )
                    }
                </InfiniteScroll>
            </div>                      
        );
    }

    componentDidUpdate(prevProps) {
        // only scroll into view if the active item changed last render
        if (this.props.selectedEntryId !== prevProps.selectedEntryId) {
          this.ensureActiveItemVisible();
        }
      }
    
    ensureActiveItemVisible() {
        var itemComponent = this.selectFactCard;
        if (itemComponent) {
            var domNode = ReactDOM.findDOMNode(itemComponent);
            this.scrollElementIntoViewIfNeeded(domNode);
        }
    }
    
    scrollElementIntoViewIfNeeded(domNode) {
        var containerDomNode = ReactDOM.findDOMNode(this);
        domNode.scrollIntoView({behavior: "smooth"});
    }
}
