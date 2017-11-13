import React from 'react';
import expect from 'expect';
import { mount } from 'enzyme';
import ListView from '../components/ListView.jsx';

describe('ListView', () => {

    let state;    
    let listView;
    let selectEntry;
    let selectedEntryId;
    
    beforeEach(() => {
        state = [{"sentence": "hi", "type":"OBJECTIVE"}]     
        selectEntry = jest.fn();
        selectedEntryId = "abc";
        listView = mount(
            <ListView
                entries={state}
                selectedEntryId={selectedEntryId}
                selectEntry={selectEntry}
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

    it('ListView renders nested components == 1', () => {
        expect(listView.find('FactCard').length).toEqual(1);
    });

    it('ListView renders nested components == number of facts', () => {
        state = []
        listView = mount(
            <ListView
                entries={state}
                selectEntry={selectEntry}
                selectedEntryId={selectedEntryId}
            />
        );
        expect(listView.find('FactCard').length).toEqual(0);
    });

    it('ListView renders nested components == number of facts', () => {
        state = [
            {"sentence": "hi", "type":"OBJECTIVE"},
            {"sentence": "hi", "type":"OBJECTIVE"},
            {"sentence": "hi", "type":"OTHER"},
            {"sentence": "hi", "type":"SUBJECTIVE"}
        ]
        listView = mount(
            <ListView
                entries={state}
                selectEntry={selectEntry}
                selectedEntryId={selectedEntryId}
            />
        );
        expect(listView.find('FactCard').length).toEqual(2);
    });
});