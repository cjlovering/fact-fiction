import React from 'react';
import expect from 'expect';
import { mount } from 'enzyme';
import ListView from '../../components/ListView.jsx';

describe('ListView', () => {

    let state;    
    let listView;
    let selectEntry;
    let selectedEntryId;
    let votes;
    let loadFunc;
    let hasMore;
    let details;
    let fetchDetails;
    let detailsShown;
    let showDetails;
    let castVote;
    
    beforeEach(() => {
        state = [{"content": "hi", "type":"OBJECTIVE", "id": "theId"}]     
        selectEntry = jest.fn();
        fetchDetails = jest.fn();
        showDetails = jest.fn();
        detailsShown = {}
        details = {};
        selectedEntryId = "abc";
        votes = {};
        hasMore = false;
        loadFunc = jest.fn();
        castVote = jest.fn();
        listView = mount(
            <ListView
                details={details}
                fetchDetails={fetchDetails}
                entries={state}
                selectedEntryId={selectedEntryId}
                votes={votes}
                selectEntry={selectEntry}
                loadFunc={loadFunc}
                hasMore={hasMore}
                detailsShown={detailsShown}
                showDetails={showDetails}
                castVote={castVote}
            />);
    });

    it('ListView requires entries prop', () => {
        expect(listView.props().entries).toBeDefined();
    });

    it('ListView requires selectEntry prop', () => {
        expect(listView.props().selectEntry).toBeDefined();
    });

    it('ListView requires selectedEntryId prop', () => {
        expect(listView.props().selectedEntryId).toBeDefined();
    });

    it('ListView requires votes prop', () => {
        expect(listView.props().votes).toBeDefined();
    });

    it('ListView requires castVote prop', () => {
        expect(listView.props().castVote).toBeDefined();
    });

    it('ListView renders nested components == 1', () => {
        expect(listView.find('FactCard').length).toEqual(1);
    });

    it('ListView renders nested components == number of facts', () => {
        state = []
        listView = mount(
            <ListView
                entries={state}
                details={details}
                fetchDetails={fetchDetails}
                selectEntry={selectEntry}
                selectedEntryId={selectedEntryId}
                votes={votes}
                loadFunc={loadFunc}
                hasMore={hasMore}
                detailsShown={detailsShown}
                showDetails={showDetails}
                castVote={castVote}
            />
        );
        expect(listView.find('FactCard').length).toEqual(0);
    });

    it('ListView renders nested components == number of facts', () => {
        state = [
            {"content": "hi", "type":"OBJECTIVE", id: "abcg"},
            {"content": "hi", "type":"OBJECTIVE", id: "abfc"},
            {"content": "hi", "type":"OTHER", id: "abec"},
            {"content": "hi", "type":"SUBJECTIVE", id: "abcd"}
        ]
        listView = mount(
            <ListView
                entries={state}
                details={details}
                fetchDetails={fetchDetails}
                selectEntry={selectEntry}
                selectedEntryId={selectedEntryId}
                votes={votes}
                loadFunc={loadFunc}
                hasMore={hasMore}
                detailsShown={detailsShown}
                showDetails={showDetails}
                castVote={castVote}
            />
        );
        expect(listView.find('FactCard').length).toEqual(2);
    });
});