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

export default class ListView extends React.Component {
    render() {
        const { 
            entries, 
            selectedEntryId, 
            selectEntry, 
            loadFunc, 
            hasMore,
            details, 
            fetchDetails, 
            detailsShown,
            showDetails, 
        } = this.props;
        // showDetails("FAK2", 44);        
        const showingDetails = id =>
            detailsShown.hasOwnProperty(id) && detailsShown[id];
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
                                key={shortid.generate()}
                            />
                        )
                    )
                }
            </InfiniteScroll>
        </div>                      
        );
    }
}

ListView.propTypes = {
    details: PropTypes.object.isRequired,
    fetchDetails: PropTypes.func.isRequired,
    entries: PropTypes.array.isRequired,
    selectedEntryId: PropTypes.string.isRequired,
    selectEntry: PropTypes.func.isRequired,
    loadFunc: PropTypes.func.isRequired,
    hasMore: PropTypes.bool.isRequired,
    detailsShown: PropTypes.object.isRequired,    
    showDetails: PropTypes.func.isRequired,
}