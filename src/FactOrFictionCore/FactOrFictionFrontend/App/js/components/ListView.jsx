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
        const { entries, selectedEntryId, selectEntry, loadFunc, hasMore } = this.props;
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
                                selectedEntryId={selectedEntryId}
                                selectEntry={selectEntry}
                                key={shortid.generate()}
                            />)
                        )
                }
                </InfiniteScroll>
            </div>
        );
    }
}

ListView.propTypes = {
    entries: PropTypes.array.isRequired,
    selectedEntryId: PropTypes.string.isRequired,
    selectEntry: PropTypes.func.isRequired,
    loadFunc: PropTypes.func.isRequired,
    hasMore: PropTypes.bool.isRequired
}